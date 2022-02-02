const dbConfig = require("../config/db.config.js");

const Sequelize = require("sequelize");
const sequelize = new Sequelize(dbConfig.DB, dbConfig.USER, dbConfig.PASSWORD, {
  host: dbConfig.HOST,
  dialect: dbConfig.dialect,
  operatorsAliases: false,

  pool: {
    max: dbConfig.pool.max,
    min: dbConfig.pool.min,
    acquire: dbConfig.pool.acquire,
    idle: dbConfig.pool.idle
  }
});

const db = {};

db.Sequelize = Sequelize;
db.sequelize = sequelize;

db.merchant = require("./merchant.model.js")(sequelize, Sequelize);
db.merchantDiscount = require("./merchant_discount.model.js")(sequelize, Sequelize);
db.user = require("./user.model.js")(sequelize, Sequelize);
db.transaction = require("./transaction.model.js")(sequelize, Sequelize);

db.sequelize.authenticate().then(async ()=>{
  defineRelations();
});

const defineRelations = () => {
  const common = (options) => ({
    ...options,
    onDelete: 'CASCADE',
    onUpdate: 'CASCADE',
  });

  db.merchant.hasMany(db.merchantDiscount, common({ foreignKey: 'merchant_id' }));
  db.merchant.hasMany(db.transaction,common({foreignKey:"merchant_id"}));

  db.merchantDiscount.belongsTo(db.merchant,  {
        foreignKey: 'merchant_id'
      });
  db.user.hasMany(db.transaction,common({foreignKey:"user_id"}));
  db.transaction.belongsTo(db.merchant,  {
    foreignKey: 'merchant_id'
  });
  db.transaction.belongsTo(db.user,  {
    foreignKey: 'user_id'
  });
};


module.exports = db;
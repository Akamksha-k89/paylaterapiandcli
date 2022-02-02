const {
    Model
  } = require('sequelize');
  module.exports = (sequelize, DataTypes) => {
    class Transaction extends Model {
  };
   Transaction.init({
        id:{
            type: DataTypes.INTEGER,
            primaryKey:true,
            autoIncrement: true        
        },
          amount: {
            type:DataTypes.FLOAT,
            allowNull:false
          }
        },
        {
            sequelize,
            modelName: 'Transaction',
      });
    return Transaction;
  };
const {
  Model
} = require('sequelize');
module.exports = (sequelize, DataTypes) => {
  class MerchantDiscount extends Model {
};
    MerchantDiscount.init({
      id:{
          type: DataTypes.INTEGER,
          primaryKey:true,
          autoIncrement: true        
      },
        discount: DataTypes.STRING,
        ActiveDate:DataTypes.DATE ,
        isActive:DataTypes.BOOLEAN
      },
      {
          sequelize,
          modelName: 'MerchantDiscount',
    });

  return MerchantDiscount;
};
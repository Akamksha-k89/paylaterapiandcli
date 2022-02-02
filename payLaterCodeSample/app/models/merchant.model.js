const {
  Model
} = require('sequelize');

module.exports = (sequelize, DataTypes) => {
    class Merchant extends Model{ };
      Merchant.init({
        id:{
          type: DataTypes.INTEGER,
          primaryKey:true,
          autoIncrement: true     
        },
        userName: { 
              type:DataTypes.STRING,
            allowNull:false,
            unique:true
          },
        address: DataTypes.STRING,
        email: {
          type: DataTypes.STRING,
          validate: {
            isEmail: {
              msg: "Must be a valid email address",
            }
          }
        }
        },
      {
        sequelize,
        modelName: 'Merchant',
     }    
      );  

    return Merchant;
  };
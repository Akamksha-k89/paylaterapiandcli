const {
    Model
  } = require('sequelize');
  
  module.exports = (sequelize, DataTypes) => {
      class User extends Model{ };
        User.init({
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
          email: {
            type: DataTypes.STRING,
            validate: {
              isEmail: {
                msg: "Must be a valid email address",
              }
            }
          },
          total_creditLimit:{
              type:DataTypes.FLOAT,
              allowNull:false
          },
          dues:{
            type:DataTypes.FLOAT,
            allowNull:false
        },
        available_credit_limit:{
            type:DataTypes.FLOAT,
            allowNull:false

        }
        },
        {
          sequelize,
          modelName: 'User',
       }    
        );  
      return User;
    };
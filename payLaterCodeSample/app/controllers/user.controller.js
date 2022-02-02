const db = require("../models");
const User = db.user;
const Op = db.Sequelize.Op;
const sequelize = db.sequelize;
// Create and Save a new User
exports.create = async (req, res) => {
  
 // Validate request
 if (!req.body.userName ||  !req.body.email  || !req.body.creditLimit) {
    res.status(400).send({
      message: "userName,email and creditLimit field cannot be empty be empty!"
    });
    return;
  }
  // Create new User
  const newUser= {
    userName: req.body.userName,
    email: req.body.email,
    total_creditLimit: parseFloat(req.body.creditLimit),
    dues: parseFloat(0.0),
    available_credit_limit: parseFloat(req.body.creditLimit),
  };
  try{
     let createdUser = await User.create(newUser);
     res.send(createdUser)
  }
catch(err)
{
  res.status(500).send({
    message:
      err.message || "Some error occurred while creating the Merchant details."
  });
}

};

// Retrieve all User from the database.
exports.dues = async (req, res) => {
  const userName = req.params.userName;
  try{
    let user = await User.findOne({where:{userName: userName}, attributes: ['dues']
  });
    if(!user)
    {
      return res.status(400).send("user is not found");
    }
    return res.status(200).send({"dues":user["dues"]})
  }catch(err)
  {
    res.status(500).send({
      message:
        err.message || "Some error occurred while creating the User details."
    });
  }
    
  };

exports.totalDues = async (req, res) => {

  try {
    let users = await User.findAll(
                      { where: { dues: { [Op.gt]:0.0 } }, 
                        attributes: { exclude: ['createdAt', 'updatedAt','id','email','creditLimit']}});
   if(!users)
   {
     return res.status(404).send({"message":"no users with dues found"});
   }
   res.send(users);
  }
  
  catch(err)
  {
    res.status(500).send({
      message:
        err.message || "Some error occurred "
    });
  }
    
  };

  exports.usedCreditLimit = async(req,res) => {
    try {
      let users = await User.findAll(
                        { where: { dues: { [Op.eq]:  db.sequelize.col('total_creditLimit') } }, 
                          attributes: { exclude: ['createdAt', 'updatedAt','id','email']}});
     if(!users)
     {
       return res.status(404).send({"message":"no users at the credit limit"});
     }
     res.send(users);
    }
    
    catch(err)
    {
      res.status(500).send({
        message:
          err.message || "Some error occurred "
      });
    }
  };

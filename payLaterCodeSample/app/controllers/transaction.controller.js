const db = require("../models");
const User = db.user;
const Merchant = db.merchant;
const Transaction = db.transaction;
// const Op = db.Sequelize.Op;

// Create and Save a new User
exports.create = async (req, res) => {
  
 // Validate request
 if (!req.body.userName ||  !req.body.merchant  || !req.body.amount) {
    return res.status(400).send({
      message: "userName,merchant and amount field cannot be empty be empty!"
    });
    
  }
  try{
        let userDetails = await User.findOne({where:{userName:req.body.userName}});
        let merchantDetails = await Merchant.findOne({where:{userName:req.body.merchant}});
        let newTransactionAmount =  parseFloat(req.body.amount);
        if(!userDetails || !merchantDetails )
        {
            return res.status(400).send({
                message: "user or merchant details not found!"
            });

        }
        let available_credit_limit =  userDetails.available_credit_limit - newTransactionAmount;
        let total_dues = userDetails.dues + newTransactionAmount;
        if(available_credit_limit<0.0)
        {
            return res.status(503).send({
                message: "Sufficient balance not avialable"
            });
        }
       let updatedUserDetails = await userDetails.update({available_credit_limit:available_credit_limit,dues:total_dues});
        
        const newTransaction = {
            merchant_id:merchantDetails.id,   
            amount:newTransactionAmount
        };
       
        let transactionCreated = await updatedUserDetails.createTransaction(newTransaction);
        res.send(transactionCreated);
        //Todo: make it commit transaction and rollback on failure
  }
catch(err)
{
  res.status(500).send({
    message:
      err.message || "Some error occurred while creating the Merchant details."
  });
}

};




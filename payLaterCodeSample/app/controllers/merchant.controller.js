const db = require("../models");
const Merchant = db.merchant;
const MerchantDiscount = db.merchantDiscount;
const Op = db.Sequelize.Op;
const moment = require('moment');
// Create and Save a new Tutorial
exports.create = async (req, res) => {
  
 // Validate request
 if (!req.body.userName ||  !req.body.discount ) {
    res.status(400).send({
      message: "userName,and discount field cannot be empty be empty!"
    });
    return;
  }
  if( !req.body.discount.match(/^100(\.(0){0,2})?$|^([1-9]?[0-9])(\.(\d{0,2}))?\%$/))
  {
    res.status(400).send({
      message: "discount field needs to be number with %"
    });
    return;
  }

  // Create a Merchant
  const newMerchant = {
    userName: req.body.userName,
    address: req.body.address||"",
    email:req.body.email
  };

  let merchantDetails = {
    merchant_id:"",
    discount:req.body.discount,
    ActiveDate: moment().format('YYYY-MM-DD hh:mm:ss'),
    isActive:true
  }
  try{
     let merchantObj = await Merchant.create(newMerchant);
     if(merchantObj)
     {
       let merchantDisObj = await merchantObj.createMerchantDiscount(merchantDetails);
        if(merchantDisObj)
        {
          console.log(merchantDisObj)
          res.send(merchantObj);
         
        }

     }
  }
catch(err)
{
  res.status(500).send({
    message:
      err.message || "Some error occurred while creating the Merchant details."
  });
}

};

// Retrieve all Merchant from the database.
exports.merchantActiveDiscount = async (req, res) => {
  const userName = req.params.userName;
  try{
      let merchantDiscountDetails = await MerchantDiscount.findOne({
        include: [
          {
            model: Merchant,
            where:{
              userName: userName
            }      
          }
        ],
        attributes:['discount'],
        where: {
          isActive:true,
        }      
      });
      if(!merchantDiscountDetails)
      {
        return res.status(404).send("merchant not found!");
      }
      res.send(merchantDiscountDetails.discount);
  }catch(err)
  {
    res.status(500).send({
      message:
        err.message || "Some error occurred while creating the Merchant details."
    });
  }
    
  };

// Find a single Tutorial with an id
exports.findOne = async (req, res) => {
  const userName = req.params.userName;
  try{
    let merchant = await Merchant.findOne({where:{userName: userName}});
    if(merchant)
    {
      let merchantDiscountDetails = await MerchantDiscount.findAll({where:{merchant_id:merchant.id}});
      res.send(merchantDiscountDetails);
    }
  }catch(err)
  {
    res.status(500).send({
      message:
        err.message || "Some error occurred while creating the Merchant details."
    });
  }
   
   
  };

// Update a Merchant by the id in the request
exports.update = async (req, res) => {
    const userName = req.params.userName;

      if(!req.body.userName)
      {
        res.status(400).send({
          message: "userName  needs to be provided"
        });
        return;
      }
      if(!req.body.discount || !req.body.discount.match(/^100(\.(0){0,2})?$|^([1-9]?[0-9])(\.(\d{0,2}))?\%$/))
      {
        res.status(400).send({
          message: "discount field needs to be provided and it is number with %"
        });
        return;

      }
      try{
         let merchantDetail = await Merchant.findOne({where:{userName:req.body.userName}});
         if(merchantDetail)
         {
          let updated = await MerchantDiscount.update({isActive:false},{where:{ merchant_id: merchantDetail.id }});
          if(updated)
          {
            let merchantDetails = {
              merchant_id:merchantDetail.id,
              discount:req.body.discount,
              ActiveDate: moment().format('YYYY-MM-DD hh:mm:ss'),
              isActive:true
            }
            let merchantDisObj = await MerchantDiscount.create(merchantDetails);
            if(merchantDisObj)
            {
                res.send(merchantDetail);
            }
          }
        }
      }
    catch(err)
    {
      res.status(500).send({
        message:
          err.message || "Some error occurred while creating the Merchant details."
      });
    }
}

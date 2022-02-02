module.exports = app => {
    const merchant = require("../controllers/merchant.controller.js");
    const user = require("../controllers/user.controller.js");
    const transaction = require("../controllers/transaction.controller.js");
    var router = require("express").Router();
  
    // Create a new Merchant
    router.post("/merchant/", merchant.create);
  
    // Retrieve merchant discount
    router.get("/merchant/GetActiveDiscount/:userName", merchant.merchantActiveDiscount);
  
    // Retrieve a merchant discounts
    router.get("/merchant/:userName", merchant.findOne);
  
    // Update merchant discount
    router.put("/merchant/", merchant.update);

    router.post("/user/",user.create);
    router.get("/user/dues/:userName",user.dues);
    router.get("/user/totalDues",user.totalDues);
    router.get("/user/nearlyUsedCreditLimit",user.usedCreditLimit);
    router.post("/transaction/",transaction.create);
    app.use('/api', router);
  };
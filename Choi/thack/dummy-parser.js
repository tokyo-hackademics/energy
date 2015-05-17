var fs = require('fs')
var faker = require('faker')
var moment = require('moment')

var file = fs.readFileSync('./dummy.json')
var stack = JSON.parse(file.toString('utf-8'))[2][1]

var result = []

var maxAmount = 0
var div = 1
for(var i = 0; i < stack.length/3/div; i++){
  var value = Math.random()
  var amount = value * value * value * Math.random() * Math.random() * Math.random() * 100
  var userName = faker.internet.userName()
  var lat = parseFloat(stack[i*3*div])
  var lng = parseFloat(stack[i*3*div+1])
  var createdAt = moment().format('YYYY-DD-MM hh:mm:ss')
  
  result.push({
    device_id:i,
    user_name:userName,
    lat:lat,
    lng:lng,
    solar:amount,
    created_at: createdAt
  })
  
  if(amount > maxAmount) maxAmount = amount
}
console.log(result.length, '/', stack.length/3)
fs.writeFileSync('./parsed-dummy.json', JSON.stringify(result))
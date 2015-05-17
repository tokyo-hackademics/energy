var mongoose = require('mongoose')
var Schema = mongoose.Schema

module.exports =  mongoose.model('Post', new Schema({
    device_id: Number,
    user_name: String,
    solar: Number,
    lat: Number,
    lng: Number,
    created_at: Date 
}))

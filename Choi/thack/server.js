var express = require('express');

var app = express();
var mongoose = require('mongoose');
var morgan = require('morgan');
var bodyParser = require('body-parser');
var methodOverride = require('method-override');

mongoose.connect('mongodb://localhost:27017/storage');

app.use(express.static(__dirname + '/public'));

app.use(morgan('dev'));
app.use(bodyParser.urlencoded({'extended':'true'}));
app.use(bodyParser.json());
app.use(methodOverride());

require('./app/models/post.js');

var server = require('http').Server(app);
var io = require('socket.io')(server);


server.listen(8080);

var fs = require('fs')
var file = fs.readFileSync('./parsed-dummy.json')
var dummy = JSON.parse(file.toString())

var compressed = []
for(var i = 0; i < dummy.length / 2; i++){
  compressed.push(dummy[i*2])
}

var jpnFilter = function(data){
  return data.filter(function(post){
    return post.lat > 24 &&
      post.lat < 46 &&
      post.lng > 124 &&
      post.lng < 148
    }).filter(function(post){
      var h = post.lat - 50
      var w = post.lng -122
      return Math.sqrt(h*h+w*w) > 18
    })
}

var jpn = jpnFilter(dummy)

var Device = require('./app/models/device.js');
var Post = require('./app/models/post.js');

io.on('connection', function (socket) {
  console.log('logged in')
  Device.find(function(err, posts){
    var data = []
    compressed.forEach(function(post){
      data.push(post)
    })
    posts.forEach(function(post){
      data.push(post)
    })
    
    var jpnData = []
    jpn.forEach(function(post){
      jpnData.push(post)
    })
    jpnFilter(posts).forEach(function(post){
      jpnData.push(post)
    })
    
    socket.emit('all', {data:data});
    
    socket.on('all', function(){
      console.log('all requested!!')
      socket.emit('all', {data:data});
    })
    
    socket.on('jpn', function(data){
      console.log('jpn requested!!')
      socket.emit('jpn', {data:jpnData})
    })
    
    socket.on('posts', function(data){
//      var device_id = data.device_id
      Post.find({device_id:0}).exec(function(err, posts){
        console.log('posts requested!!')
        socket.emit('posts', {data:posts})
      })
    })
  })
});
require('./app/routes.js')(app, io);
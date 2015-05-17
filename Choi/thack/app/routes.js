var Post = require('./models/post.js');
var Device = require('./models/device.js');
var _ = require('lodash')
var moment = require('moment')

var mutateOne = function(post){
  var aPost = JSON.parse(JSON.stringify(post));
  aPost.created_at = moment(post.created_at).format('YYYY-DD-MM hh:mm:ss').toString()
  return aPost
}

var mutateAll = function(posts){
  return posts.map(mutateOne)
}

module.exports = function(app, io){
  app.get('/api/devices', function(req,res){
    Device.find(function(err, devices){
      if(err) res.send(err);
      
      var aDevices = mutateAll(devices)
      
      res.json({posts:aDevices});
    })
  })
  
  app.delete('/api/devices/:id', function(req, res){
      Device.findById(req.params.id, function(err, post){
          if(err) res.send(err);
          
          post.remove(function(err, post){
            res.json({result:'OK'})
          })
      });
  })
  
  app.get('/api/posts', function(req, res){
      Post.find(function(err, posts){
          if(err) res.send(err);
          
          var aPosts = mutateAll(posts)
          
          res.json({posts:aPosts});
      });
  });
  
  app.delete('/api/posts/:id', function(req, res){
      Post.findById(req.params.id, function(err, post){
          if(err) res.send(err);
          
          post.remove(function(err, post){
            res.json({result:'OK'})
          })
      });
  })

  app.post('/api/posts', function(req, res){  
    var device_id = parseInt(req.body.device_id),
    user_name = req.body.user_name,
    lat = parseFloat(req.body.lat),
    lng = parseFloat(req.body.lng),
    solar = parseFloat(req.body.solar)
    
    var isNum = function(target){
      return _.isNumber(target) && !_.isNaN(target)
    }
    
    if(
      !isNum(device_id) ||
      !_.isString(user_name) ||
      !isNum(lat) ||
      !isNum(lng) ||
      !isNum(solar)
      ) {
        console.log(req.body)
        res.send({err:'validation_failed'}, 400)
        return
      }
    
    Post.create({
      device_id:device_id,
      user_name:user_name,
      lat:lat,
      lng:lng,
      solar:solar,
      created_at: new Date()
    }).then(function(post){
      Device.findOne({device_id:device_id})
        .exec(function(err, device){
          if(err) res.send(err)
          
          if(device == null){
            Device.create({
              device_id:device_id,
              user_name:user_name,
              lat:lat,
              lng:lng,
              solar:solar,
              created_at: new Date()
            }).then(function(){
              var aDevice = mutateOne(device)
              io.sockets.emit('update', {data:aDevice})
              
              res.json({result:'OK'})
            })
          }else{
            device.update({
              user_name:user_name,
              lat:lat,
              lng:lng,
              solar:device.solar + solar,
              created_at: new Date()
            }).exec(function(){
              Device.findOne({device_id:device_id})
                .exec(function(err, device){
                  if(err) res.send(err)
                  
                  var aDevice = mutateOne(device)
                  io.sockets.emit('update', {data:aDevice})
                  res.json({result:'OK'})
                })
            })
          }
        })
    });
  });
  
  app.get('/globe', function(req,res){
    res.sendfile('./public/globe.html')
  })
  

  app.get('/', function(req,res){
      res.sendfile('./public/index.html');
  });

};
angular.module('dashboard', [])
  .constant('rootUrl', '/api/')
  .value('$socket', socket)
  .factory('Device', function($http, rootUrl){
    var all = function(){
      var url = rootUrl + 'devices'
      
      return $http.get(url)
    }
    
    var destroy = function(id){
      var url = rootUrl + 'devices/' + id
      
      return $http.delete(url)
    }
    
    return {
      all: all,
      destroy: destroy
    }
  })
  .factory('Post', function($http, rootUrl){
    var all = function(){
      var url = rootUrl + 'posts'
      
      return $http.get(url)
    }
    
    var store = function (params){
      var url = rootUrl + 'posts'
      
      return $http.post(url, params)
    }
    
    var destroy = function (id){
      var url = rootUrl + 'posts/' + id
      
      return $http.delete(url)
    }
    
    return {
      all: all,
      store: store,
      destroy: destroy
    }
  })
  .controller('MainController', function($socket, Device, Post, $timeout){
    var main = this
    
    Device.all()
      .success(function(data){
        console.log(data)
        main.posts = data.posts
      })
      
    main.save = function(){
      var params = {
        device_id:main.device_id,
        user_name:main.user_name,
        lat:main.lat,
        lng:main.lng,
        solar:main.solar
      }
      
      Post.store(params)
        .success(function(data){
        })
    }
      
    main.send = function(){
      console.log('clicked!!')
      $socket.emit('jpn', {'msg':'Msg sent!'})
    }
    
    $socket.on('update', function(data){
      console.log('updated')
      console.log(data.data)
      var post = data.data
      
      var exists = false
      for(var i = 0; i < main.posts.length; i++){
        if(main.posts[i].device_id == post.device_id){
          main.posts[i] = post
          exists = true
        }
      }
      if(!exists)main.posts.push(post)
      
      $timeout(function(){},0)
    });
    
    main.destroy = function(id){
      Device.destroy(id)
        .success(function(data){
          main.posts = main.posts.filter(function(post){
            return post._id != id
          })
        })
    }
  })
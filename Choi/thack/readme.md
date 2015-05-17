# Energy

# Transaction

## Server URL
`192.168.3.38:8080`

## Dashboard
http://192.168.3.38:8080/

## Device-Server API(HTTP)

###データの保存

#### Request Header
POST  
http://192.168.3.38:8080/api/posts  
Content-Type : `application/json` or `application/x-www-form-urlencode`

#### Request Body
``` json
{"device_id":Integer,"user_name":String,"lat":Float,"lng":Float,"solar":Float}
```

> Example
``` json
{"device_id":21,"user_name":"we","lat":30.535,"lng":124.365,"solar":84.34}
```

## Client-Server API(Web Socket)

### すべてのデータの送信

#### Event name
`all`

#### Data example
``` json
{
  "data":[
    {
      "created_at": "2015-16-05 04:51:16",
      "device_id": 6,
      "user_name": "Wallace50",
      "lat": -32.4744,
      "lng": -152.0251,
      "solar": 90.5637891497463
    },
    {
      "created_at": "2015-16-05 04:51:16",
      "device_id": 4,
      "user_name": "Rory_Lowe69",
      "lat": 8.3874,
      "lng": -115.6247,
      "solar": 42.608364508487284
    }
  ]
}
```

### 部分的なデータの送信

#### Event name
`update`

#### Data example
``` json
{
  "data":{
    "created_at": "2015-16-05 04:51:16",
    "device_id": 4,
    "user_name": "Rory_Lowe69",
    "lat": 8.3874,
    "lng": -115.6247,
    "solar": 42.608364508487284
  }
}
```



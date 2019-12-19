# LocationAPI

LocationApi is API to add, delete, edit and get locations based on IP address and URL. It targets .NET Framework 4.8 and it's based on MongoDb. </br>

**NOTE:** API do not store results with empty IP address as it's more important for determinig location. However API accepts empty IP addresses or URLs and tries to resolve them while adding. So when IP address couldn't not be resolved from URL it returns error. When URL couldn't not be resolved from IP address API stores it as null. </br>

**NOTE2:** API allows to store results that many IP addresses points to same URL. However it do not allow to store results with same IP address pointing to different URL and assumes it as duplicates. 

## How to install?

Download image from dockerhub and run as docker container. Docker for Windows installed is required. ```docker pull lukzap/location-api```. </br>
or </br>
Build solution with ```dotnet``` and run as an app. MongoDb installed is required.


## How to use?

#### GET
{baseUrl}/api/geolocation?q={query} </br>
ex. http://localhost:27011/api/geolocation?q=1.1.1.1 or http://localhost:27011/api/geolocation?q=onet.pl

Returns geolocation based on IP address or URL.

*example response:*

```json
{ 
   "Id":"5dfabfb5e028a82a1401c832",
   "Url":"waw.pl",
   "IP":"188.128.255.234",
   "IPType":"IPv4",
   "Continent":"Europe",
   "Country":"Poland",
   "City":"Sopot",
   "Lat":112.0,
   "Lng":170.0
}
```

#### POST
{baseUrl}/api/geolocation </br>
ex. http://localhost:27011/api/geolocation

Adds geolocation. 

*example request:*

```json
{ 
   "Url":null,
   "IP":"188.128.255.254",
   "IPType":"IPv4",
   "Continent":"Europe",
   "Country":"Poland",
   "City":"Sopot",
   "Lat":112.0,
   "Lng":170.0
}
```
Returns geolocation based on IP address or URL. 

*example response:*

```json
{ 
   "Id":"5dfabfb5e028a82a1401c832",
   "Url":"waw.pl",
   "IP":"188.128.255.234",
   "IPType":"IPv4",
   "Continent":"Europe",
   "Country":"Poland",
   "City":"Sopot",
   "Lat":112.0,
   "Lng":170.0
}
```

#### PUT
{baseUrl}/api/geolocation?id={id} </br>
ex. http://localhost:27011/api/geolocation?id=5df7f47de028a83f84642980

Updates geolocation.  

*example request:*

```json
{ 
   "Url":null,
   "IP":"188.128.255.254",
   "IPType":"IPv4",
   "Continent":"Europe",
   "Country":"Poland",
   "City":"Gdynia",
   "Lat":112.0,
   "Lng":170.0
}
```
Returns geolocation based on IP address or URL. 

*example response:*

```json
{ 
   "Id":"5df7f47de028a83f84642980",
   "Url":"waw.pl",
   "IP":"188.128.255.234",
   "IPType":"IPv4",
   "Continent":"Europe",
   "Country":"Poland",
   "City":"Gdynia",
   "Lat":112.0,
   "Lng":170.0
}
```

#### DELETE
{baseUrl}/api/geolocation?id={id} </br>
ex. http://localhost:27011/api/geolocation?id=5df7f47de028a83f84642980

Deletes geolocation.  

Returns deleted geolocation Id. 

*example response:*

```
5df7f47de028a83f84642980
```

## TODO's
- fix resoving URLs with more meaningful results - not like "waw.pl"
- true integration tests - not controller tests (problems with self-hosting)
- advanced validations of request based on geolocations
- add Swagger

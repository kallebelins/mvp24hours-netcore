#Command 
docker run --name mongodb -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=user -e MONGO_INITDB_ROOT_PASSWORD=123456 mongo

#ConnectionString
mongodb://user:123456@localhost:27017

#Tools
https://robomongo.org/

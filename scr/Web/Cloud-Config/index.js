fs = require('fs');
module.exports.serverconfig =
{
    hostname: "www.cloud-social.com",
    
    hostport: 8081,
    rootpath: "/DataService.svc/",
    globalheaders: {
        "Content-Type": "application/json"
    }
}
module.exports.appconfig = [];
//默认 云渠
module.exports.appconfig['*'] = {
    AppId: "id",
    AppKey: "159753"
};
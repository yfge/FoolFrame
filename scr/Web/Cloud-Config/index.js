fs = require('fs');
module.exports.serverconfig =
{
    hostname: "localhost",
    
    hostport: 1407,
    rootpath: "/DataService.svc/",
    globalheaders: {
        "Content-Type": "application/json"
    }
}
module.exports.appconfig = [];
//默认 云渠
module.exports.appconfig['*'] = {
    AppId: "3C53E09F-0CE9-4289-BA7E-C918A5F0C076",
    AppKey: "159753"
};
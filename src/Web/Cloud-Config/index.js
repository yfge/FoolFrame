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
    AppId: "E4BEE30B-F38B-41D3-8B83-4C08E5E25FDE",
    AppKey: "159753"
};

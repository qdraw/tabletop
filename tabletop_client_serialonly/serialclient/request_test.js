
const https = require('https');


const options = {
  hostname: 'apollo.qdraw.eu',
  port: 443,
  path: '/',
  method: 'GET'
};


https.get(options , (res) => {
  console.log('statusCode:', res.statusCode);
  console.log('headers:', res.headers);

  res.on('data', (d) => {
    process.stdout.write(d);
  });

}).on('error', (e) => {
  console.error(e);
});



// const request = require('request');
//
// const querystring = require('querystring');
// var formquery = {
// 	"name" : "test",
// 	"status" : 1
// }
// request({
// 		headers: {
// 			'Content-Length': querystring.stringify(formquery).length,
// 			'Content-Type': 'application/x-www-form-urlencoded',
// 			'Authorization': 'Bearer kHZ6ody2nQ9dmcMSCk5m'
// 		},
// 		uri: 'https://demo.colours.ai/tabletop/api/update',
// 		body: querystring.stringify(formquery),
// 		method: 'POST',
// 		}, function (err, res, body) {
// 			console.log(err);
// 			console.log(body);
//
// 			if (res.statusCode === 200) { // success
// 				console.log("succes");
// 				resolve();
// 			}
//
// 			if (res.statusCode >= 400) { // error
// 				console.log("fail");
// 				console.log(body);
// 				reject()
// 			}
// 	});

var SerialPort = require('serialport');
const querystring = require('querystring');
const request = require('request');

require('dotenv').load();

if (process.env.BEARER === undefined) {
    throw "missing bearer";
}

if (process.env.SERIALPORT === undefined) {
    throw "missing SERIALPORT";
    process.exit();
}

var serialPort = new SerialPort(process.env.SERIALPORT, {
  baudRate: 9600
});

serialPort.on("open", function () {
	console.log('open');
	serialPort.on('data', function(data) {
		var dataString =  data.toString('ascii');
		if (dataString.indexOf("detected")) {
			console.log("detected");
            httpUpdate();
		};
	});
});


function httpUpdate() {
	return new Promise(
		function(resolve, reject) {
			var formquery = {
				"name" : "test",
				"status" : 1
			}

			request({
					headers: {
						'Content-Length': querystring.stringify(formquery).length,
						'Content-Type': 'application/x-www-form-urlencoded',
                        'Authorization': 'Bearer ' + process.env.BEARER
					},
					uri: 'http://demo.colours.ai/tabletop/api/update',
					body: querystring.stringify(formquery),
					method: 'POST',
					}, function (err, res, body) {
                        console.log(err);
						console.log(body);

						if (res.statusCode === 200) { // success
							console.log("succes");
                            resolve();
						}

						if (res.statusCode >= 400) { // error
							console.log("fail");
							console.log(body);
							reject()
						}
				});
		}
	);
}

var https = require('https')

var options = {
    hostname: 'qdraw.eu',
    port: 443,
    path: '/',
    method: 'GET',
    secureProtocol: "TLSv1_1_method",
    rejectUnauthorized: false,
      requestCert: true,
      agent: false
};

https.request(options, res => {
  let body = ''
  res.on('data', d => body += d)
  res.on('end', () => {
      console.log("hi");
    // data = JSON.parse(body);
    // console.log('SSL Version: ' + data.tls_version);
});
}).on('error', err => {
  // This gets called if a connection cannot be established.
  console.warn(err)
}).end()

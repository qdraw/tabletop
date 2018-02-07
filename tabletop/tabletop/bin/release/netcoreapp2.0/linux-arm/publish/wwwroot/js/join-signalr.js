
window.signalr = {
// ReSharper disable once UnusedParameter
    invoke: function (connection, method, ...args) {
        if (!window.signalr.isConnected) {
            return;
        }
        var argsArray = Array.prototype.slice.call(arguments);
        connection.invoke.apply(connection, argsArray.slice(1))
            .then(result => {
                console.log("invocation completed successfully: " + (result === null ? '(null)' : result));

                //if (result) {
                //    addLine('message-list', result);
                //}
            })
            .catch(err => {
                console.log(err);
                //addLine('message-list', err, 'red');
            });
    },


    signalr: function() {
        var transportType = signalR.TransportType[window.signalr.getParameterByName("transport")] || signalR.TransportType.WebSockets;
        var url = window.updateIsFreeEnv.url || `http://${document.location.host}/datahub`;
        var http = new signalR.HttpConnection(url, { transport: transportType });
        window.signalr.connection = new signalR.HubConnection(http);

        window.signalr.connection.on("Pong", msg => {
            console.log("Pong > ", msg);
            this.pongDate = Date.now();
        }),

        window.signalr.connection.on("Update", msg => {
            //console.log("Update > ", msg);
            window.updateIsFree.index(msg);
       });


        window.signalr.connection.on("EventsRecent", msg => {
            window.draw.drawD3(msg.amountOfMotions);
        });


        window.signalr.connection.onClosed = e => {
            this.isConnected = false;
            if (e) {
                console.log(e);
            }
            else {
                console.log("e");
            }
        }


        window.signalr.connection.start()
            .then(() => {
                window.signalr.isConnected = true;
                //console.log("Connected successfully");
                window.signalr.invoke(window.signalr.connection, "JoinGroup", window.updateIsFreeEnv.name);
            })
            .catch(err => {
                console.log("Connection fails");
                console.log(err);
            }); 
    },

    pongDate : 0,

    pongSend: function () {

        var latestpong = Date.now() - this.pongDate;
        var longTimeAgo = false;
        if (latestpong >= 2000) {
            longTimeAgo = true;
        }

        if (this.isConnected) {
            window.signalr.connection.invoke("Pong","true");
        }
        return longTimeAgo;
    },

    isConnected : false,

    getParameterByName: function (name, url) {
        if (!url) {
            url = window.location.href;
        }
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }
};

window.signalr.signalr();




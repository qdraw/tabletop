if (window.updateIsFreeEnv === undefined) {
    window.updateIsFreeEnv = {
        "url": "isfree_name_tafelvoetbal.json?" + Date.now(),
        "updateInterval": null,
        "name": "tafelvoetbal"
    };
}

var updateIsFree = {
    index : function () {
        if (window.updateIsFreeEnv.url !== undefined && window.updateIsFreeEnv.url !== null ) {
            var updateInterval = window.updateIsFreeEnv.updateInterval;
            if (!isNaN(updateInterval) && updateInterval !== null && updateInterval !== undefined ) {
                setInterval(function(){
                    updateIsFree.start();
                }, updateInterval);
            }
        }
    },

    start : function () {
       var url = window.updateIsFreeEnv.url;
       updateIsFree.loadJSON(url,
                function(data) {
                    if (data.dateTime !== undefined && data.isFree !== undefined) {
                        var d = new Date(data.dateTime + "+00:00");
                        document.querySelector('#latestactivity').innerHTML = "Latest activity on location: "  + d.toLocaleDateString("NL-nl") + " " + d.toLocaleTimeString("NL-nl");

    					if (data.isFree) {
                            document.querySelector('.databox').classList.remove("red");
                            document.querySelector('.databox').classList.add("green");

                            document.querySelector("link[rel*='icon']").href = "img/favicon_free.png";
                            document.title = document.title.replace(/^(is in use)|(free)|(Loading)/, "free");

                            document.querySelector('#online-indicator').className = "circle circle-big circle-green";
                            document.querySelector('#offline-indicator').className = "circle circle-big circle-blank";
                            document.querySelector('#status').innerHTML = "free";

                            if (document.querySelectorAll("#data.border").length >= 1) {
                                document.querySelector('#data').classList.remove("border-red");
                                document.querySelector('#data').classList.add("border-green");
                            }

    					}
                        if (!data.isFree) {
                            document.querySelector('.databox').classList.remove("green");
                            document.querySelector('.databox').classList.add("red");

                            document.querySelector("link[rel*='icon']").href = "img/favicon_inuse.png";
                            document.title = document.title.replace(/^(is in use)|(free)|(Loading)/, "is in use");

                            document.querySelector('#online-indicator').className = "circle circle-big circle-blank";
                            document.querySelector('#offline-indicator').className = "circle circle-big circle-red";
                            document.querySelector('#status').innerHTML = "in use";

                            if (document.querySelectorAll("#data.border").length >= 1) {
                                document.querySelector('#data').classList.remove("border-green");
                                document.querySelector('#data').classList.add("border-red");
                            }

                        }
                    }
                    if (data.dateTime === undefined && data.isFree === undefined) {
                        updateIsFree.resetAll();
                    }


                },
                function (xhr) {
                    updateIsFree.resetAll();
                    console.error(xhr);
               }
      );
    },

    resetAll:  function () {
        document.querySelector('.databox').classList.remove("red");
        document.querySelector('.databox').classList.remove("green");
        document.querySelector('#data').classList.remove("border-red");
        document.querySelector('#data').classList.remove("border-green");
        document.querySelector('#status').innerHTML = "...";
        document.querySelector('#online-indicator').className = "circle circle-big circle-blank";
    },

    loadJSON: function (path, success, error) {
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function()
        {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    if (success)
                        success(JSON.parse(xhr.responseText));
                } else {
                    if (error) {
                          console.log("err");
                        error(xhr);
                          }
                }
            }
        };
        xhr.open("GET", path, true);
        xhr.send();
    }

};

updateIsFree.index();

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
            updateIsFree.start();
            var updateInterval = window.updateIsFreeEnv.updateInterval;
            if (!isNaN(updateInterval) && updateInterval !== null && updateInterval !== undefined ) {
                setInterval(function(){
                    updateIsFree.start();
                }, updateInterval);
            }
        }
    },

    start : function () {
        url = window.updateIsFreeEnv.url;
       updateIsFree.loadJSON(url,
                function(data) {
                    if (data.dateTime !== undefined && data.isFree !== undefined) {
                        var d = new Date(data.dateTime + "+00:00");
                        document.querySelector('#latestactivity').innerHTML = "Laatste activiteit was op: "  + d.toLocaleDateString("NL-nl") + " " + d.toLocaleTimeString("NL-nl");
    					document.querySelector('#isfree').innerHTML = "Is Free?: "  + data.isFree;

    					if (data.isFree) {
                            document.querySelector(".databox").style.backgroundColor = "#0DFFD5";
                            document.querySelector("link[rel*='icon']").href = "img/favicon_free.png";
    					    document.title = "is Free now - tabletop";

    					}
                        if (!data.isFree) {
                            document.querySelector(".databox").style.backgroundColor = "#FF807F";
                            document.querySelector("link[rel*='icon']").href = "img/favicon_inuse.png";
                            document.title = "is in use - tabletop";
                        }

                    }
                },
               function(xhr) { console.error(xhr); }
      );
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
  },

};

updateIsFree.index();

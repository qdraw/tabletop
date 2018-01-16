
if (window.envName === undefined) {
    window.envName = "tafelvoetbal"
}

function loadJSON(path, success, error) {
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

function updateRecentByName() {
    var url = "api/getRecentByName?name=" + window.envName;
    // var url = "getRecentByName_name_tafelvoetbal.json?" + Date.now();
	loadJSON(url,
			 function(data) {
                 listOfData = parseData(data);

				 if (document.querySelectorAll("#preloader").length >= 1) {
					 document.querySelector("#preloader").style.display = "none";
				 }
				 document.querySelector(".databox").style.display = "block";
			     document.querySelector(".notes").style.display = "block";

                 draw(listOfData);
             },
            function(xhr) { console.error(xhr); }
   );
}
updateRecentByName();
updateIsFree();

function updateIsFree() {
    url = "api/isfree?name=" + window.envName;
   loadJSON(url,
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
                console.log(data);
            },
           function(xhr) { console.error(xhr); }
  );
}

setInterval(function(){
   updateRecentByName();
}, 100000);

setInterval(function(){
   updateIsFree();
}, 20000);




function returnNowDate() {
   var now = new Date();
   var utc_timestamp_unix = Date.UTC(now.getUTCFullYear(),now.getUTCMonth(), now.getUTCDate() ,
        now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds())/1000;
   return utc_timestamp_unix;
}

function parseData(data) {

   var utc_timestamp_unix = returnNowDate();

   var eighthoursago_unix = Math.floor(utc_timestamp_unix - 28800); // in seconds
   var steps = 60; // in seconds

   function checkForData(start_search_unix) {
       for (var i = 0; i < data.length; i++) {

           var d = new Date(data[i].dateTime + "+00:00");

            d_unixtimestamp = Math.floor(d/1000);
            if (d_unixtimestamp >= start_search_unix && d_unixtimestamp <= start_search_unix+steps) {
                return i;
            }
       }
       return -1;
   }
   var listOfData = [];


   for (var i = eighthoursago_unix; i < utc_timestamp_unix-1; i = i + steps) {
       var arrrayInt  = -1;
       arrrayInt = checkForData(i);
       var dateI = new Date(i*1000);

       if (arrrayInt >= 0) {
           listOfData.push({
                "label":  Math.ceil(Number(dateI)/1000),
                "value":data[arrrayInt].weight + 1
            });
       }
       else {

           listOfData.push({
                "label":  Math.ceil(Number(dateI)/1000),
                "value": 0
            });
       }
   }
   // console.log(listOfData);
   return listOfData;
}



function draw(data) {

   var margin = {top: 0, right: 0, bottom: -8, left: 0},
   width = 800 + (margin.right - margin.left),
   height = 300 + (margin.top - margin.bottom);

   var bars = d3.select("#data")
       .attr("preserveAspectRatio", "xMinYMin meet")
       .attr("viewBox", "0 0 " + (width + margin.left + margin.right) + " " + height)
       // .attr("width", width + margin.left + margin.right)
       .attr("class","bar")
       // .attr("height", height + margin.top + margin.bottom)
       .attr("fill", "#000")
       .attr("transform","translate(" + margin.left + "," + margin.top + ")")
       .selectAll("rect")
       .data(data);


   // set the ranges
   var x = d3.scaleBand()
       .range([0, width]);
       // .padding(0.1);
   var y = d3.scaleLinear()
       .range([height, 0]);

   // Scale the range of the data in the domains
   x.domain(data.map(function(d) { return d.label; }));
   y.domain([0, d3.max(data, function(d) { return d.value; })]);

   // if (svglength === 0) {}
       // add the x Axis
   svg = d3.select("#data");
   svg.append("g")
       .attr("transform", "translate(0," + height + ")")
       .attr("stroke", "#fff")
       .call(d3.axisBottom(x));

   // add the y Axis
   // svg.append("g")
   //     // .attr("stroke", "#000")
   //     .call(d3.axisLeft(y));

   // enter and update selection
   bars
     .enter().append("rect")
     .merge(bars)
     .attr("x", function(d) { return x(d.label); })
     .attr("width", x.bandwidth())
     .attr("y", function(d) { return y(d.value); })
     .attr("height", function(d) { return height - y(d.value); });
     // .text(function (d) {return d;});

   // exit selection
   bars
       .exit().remove();
}


if (window.envName === undefined) {
    window.envName = "tafelvoetbal";
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
    // var url = "api/getRecentByName?name=" + window.envName;
    var url = "EventsOfficehours_name_tafelvoetbal_date_0.json?" + Date.now();
	loadJSON(url,
			 function(data) {
                 // listOfData = parseData(data);
                 //
				 // if (document.querySelectorAll("#preloader").length >= 1) {
					//  document.querySelector("#preloader").style.display = "none";
				 // }
				 // document.querySelector(".databox").style.display = "block";
                 data = draw.checkData(data);
                 draw.unHideElements(data);

                 draw.drawD3(data);
             },
            function(xhr) { console.error(xhr); }
   );
}
updateRecentByName();
// updateIsFree();

// function updateIsFree() {
//     url = "api/isfree?name=" + window.envName;
//    loadJSON(url,
//             function(data) {
//                 if (data.dateTime !== undefined && data.isFree !== undefined) {
//                     var d = new Date(data.dateTime + "+00:00");
//                     // document.querySelector('#latestactivity').innerHTML = "Laatste activiteit was op: "  + d.toLocaleDateString("NL-nl") + " " + d.toLocaleTimeString("NL-nl");
// 					document.querySelector('#isfree').innerHTML = "Is Free?: "  + data.isFree;
//
// 					if (data.isFree) {
//                         document.querySelector(".databox").style.backgroundColor = "#0DFFD5";
//                         document.querySelector("link[rel*='icon']").href = "img/favicon_free.png";
// 					    document.title = "is Free now - tabletop";
//
// 					}
//                     if (!data.isFree) {
//                         document.querySelector(".databox").style.backgroundColor = "#FF807F";
//                         document.querySelector("link[rel*='icon']").href = "img/favicon_inuse.png";
//                         document.title = "is in use - tabletop";
//                     }
//
//                 }
//                 console.log(data);
//             },
//            function(xhr) { console.error(xhr); }
//   );
// }

setInterval(function(){
   updateRecentByName();
}, 100000);

// setInterval(function(){
//    updateIsFree();
// }, 20000);




var draw = {
    checkData : function(data){
        var drawData = null;
        if (data.amountOfMotions !== undefined) {
            drawData =  data.amountOfMotions;
        }
        // var parseTime = d3.timeParse("%H:%M");
        // for (var i = 0; i < drawData.length; i++) {
        //     // drawData[i].label = parseTime(drawData[i].label);
        //     // drawData[i].label = new Date(drawData[i].startDateTime);
        // }

        return drawData;
    },
    unHideElements : function(data){
        if (data === null) {
            alert("data fails");
            return;
        }
        document.querySelector('.databox').style.display = "block";
        document.querySelector('#preloader').style.display = "none";

    },

    returnNowDate: function() {
       var now = new Date();
       var utc_timestamp_unix = Date.UTC(now.getUTCFullYear(),now.getUTCMonth(), now.getUTCDate() ,
            now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds())/1000;
       return utc_timestamp_unix;
   },


    drawD3: function(data) {
        if (data === null) {
            throw "err";
        }
        console.log(data);

       var margin = {top: 20, right: 0, bottom: 30, left: 0},
       width = 800 + (margin.right - margin.left),
       height = 300 + (margin.top - margin.bottom);

       d3.select("#data")
           .attr("preserveAspectRatio", "xMinYMin meet")
           .attr("viewBox", "0 0 " + (width + margin.left + margin.right) + " " + (height +40));

       var bars = d3.select("#data .bars")
           // .attr("height", height + margin.top + margin.bottom)
           .attr("fill", "#000")
           // .attr("transform","translate(" + margin.left + "," + margin.top + ")")
           .selectAll("rect")
           .data(data);


       // set the ranges
       var x = d3.scaleBand()
           .range([0, width]);
           // .padding(0.1);
       var y = d3.scaleLinear()
           .range([height, 0]);

       // Scale the range of the data in the domains
       var i = 0;
       x.domain(data.map(function(d) {return d.label; }));
       y.domain([0, d3.max(data, function(d) { return d.weight; })]);

       // add the x Axis
       svg = d3.select("#data");
       svg.append("g")
           .attr("class", "x_time")
           .attr("transform", "translate(0," + height + ")")
           .attr("stroke", "#000")
           .call(
               d3.axisBottom(x)
                .tickFormat(function(d,i) {
                    if (d.indexOf(":00") >= 0 || d.indexOf(":30") >= 0) {
                        return d;
                    }
                    // if ((i % 10) == 0) {
                    //     console.log(i);
                    //     return d;
                    // }
                    else {
                        return null;
                    }
                    // return d % 1 ? null : d;
                })
                // .tickFormat(d3.timeFormat("%H"))
            )
            .selectAll("text")
            .style("text-anchor", "end")
            .attr("dx", "-.8em")
            .attr("dy", ".15em")
            .attr("transform", "rotate(-90)");



       // var yAxis = d3.axisLeft(y)
       //   .tickSizeInner(5)
       //   .tickSizeOuter(10)
       //   .ticks(20, "s");
       //
       // svg.append('g')
       //    .classed('y axis', true)
       //    .call(yAxis);

       // // // add the y Axis
       // svg.append("g")
       //     .attr("stroke", "#000")
       //     .call(d3.axisLeft(y).tickValues([0, 75, 150, 1000, 2500, 5000, 10000]) );

       // enter and update selection
       bars
         .enter().append("rect")
         .merge(bars)
         .attr("x", function(d) { return x(d.label); })
         .attr("width", x.bandwidth())
         .attr("y", function(d) { return y(d.weight); })
         .attr("height", function(d) { return height - y(d.weight); });
         // .text(function (d) {return d;});

       // exit selection
       bars
           .exit().remove();
    }
};

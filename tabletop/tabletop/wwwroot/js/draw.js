
if (window.drawEnv === undefined) {
    window.drawEnv = {
        "url": "EventsOfficehours_name_tafelvoetbal_date_0.json?" + Date.now(),
        "updateInterval": null,
        "name": "tafelvoetbal",
        "relativeDate": 0
    };
}


var draw = {

    data : {},

    pageLoaded: false,

    index : function () {

        draw.start();

        var updateInterval = window.drawEnv.updateInterval;
        if (!isNaN(updateInterval) && updateInterval !== null && updateInterval !== undefined && updateInterval !== false ) {

            setInterval(function(){
                if (window.drawEnv.updateInterval !== false) {
                    draw.start();
                }
            }, updateInterval);
        }

    },

    start : function () {
        // var url = "api/getRecentByName?name=" + window.envName;
        // var url = "EventsOfficehours_name_tafelvoetbal_date_0.json?" + Date.now();
        var url = window.drawEnv.url.replace(/&amp;/ig,"&");
    	draw.loadJSON(url,
    			 function(data) {
                     data = draw.checkData(data);
                     draw.unHideUpdateElements(data);
                     // data = draw.parseDataUnique(data);
                     draw.drawD3(data);
                 },
                function(xhr) { console.error(xhr); }
       );
   },

    checkData : function(data){
        draw.data = data;
        // console.log(draw.data);

        var drawData = null;
        if (data.amountOfMotions !== undefined) {
            drawData =  data.amountOfMotions;
        }
        return drawData;
    },

    parseDataUnique : function(drawData){
        // Test function for sorting unique data
        var uniqueDrawData = [];

        for (var i = 0; i < drawData.length; i++) {
            var label = drawData[i].label;

            if (uniqueDrawData[label] === undefined) {
                if (Number(drawData[i].label.substr(0, 2)) > 8 && Number(drawData[i].label.substr(0, 2)) < 21 ) {
                    uniqueDrawData[label] = {
                        weight : drawData[i].weight,
                        // weight : 1,
                        label : drawData[i].label
                    };
                }
            }
            else {
                // if (drawData[i].weight >= 1) {
                //     uniqueDrawData[label].weight += 1;
                // }
                uniqueDrawData[label].weight += drawData[i].weight;
            }
        }
        var listOfUniqueDrawData = [];
        for (var item in uniqueDrawData) {
            listOfUniqueDrawData.push(uniqueDrawData[item]);
        }
        console.log(listOfUniqueDrawData);
        return listOfUniqueDrawData;
    },


        // var parseTime = d3.timeParse("%H:%M");
        // for (var i = 0; i < drawData.length; i++) {
        //     // drawData[i].label = parseTime(drawData[i].label);
        //     drawData[i].label = new Date(drawData[i].startDateTime);
        // }

    unHideUpdateElements : function(data){
        if (data === null) {
            alert("data fails");
            return;
        }
        document.querySelector('.databox').style.display = "block";
        document.querySelector('#preloader').style.display = "none";

        // console.log(window.drawEnv.relativeDate);
        if (window.drawEnv.relativeDate === 0) {

            document.querySelector('#data').classList.add("border");
            // console.log("hi");
        }

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

    leadingZero: function (value) {
        if (value < 9) {
            return "0"+ value;
        }
        return String(value);
    },


    returnNowDate: function() {
       var now = new Date();
		return Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(),
		   now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds()) / 1000;
   },

    drawD3: function(data) {
        if (data === null) {
            console.log("no update");
            return;
        }

       var margin = {top: 25, right: 0, bottom: 30, left: 30},
       width = 800 + (margin.right - margin.left),
       height = 300 + (margin.top - margin.bottom);

       window.d3.select("#data")
           .attr("preserveAspectRatio", "xMinYMin meet")
           .attr("viewBox", "0 0 " + (width + margin.left + margin.right+5) + " " + (height +margin.top+15));

       var bars = window.d3.select("#data .bars")
           .attr("fill", "#fff")
           .attr("transform","translate(" + (margin.left+20) + "," + 0 + ")")
           .selectAll("rect")
           .data(data);


       // set the ranges
       var x = window.d3.scaleBand()
           .range([0, width]);
           // .padding(0.1);
       var y = window.d3.scaleLinear()
           .range([height, 0]);

       // Scale the range of the data in the domains
       x.domain(data.map(function(d) {return d.label; }));
       y.domain([0, window.d3.max(data, function(d) { return d.weight; })]);

       var svg = window.d3.select("#data .x_time")
           .remove();

       // add the x Axis
       svg = window.d3.select("#data");
       svg.append("g")
           .attr("class", "x_time")
           .attr("transform", "translate("+ (margin.left+3) + "," + (height+5) + ")")
           .call(
               window.d3.axisBottom(x)
                .tickFormat(function(d) {
                    if (typeof(d) === "string") {
                        if (d.indexOf(":00") >= 0 || d.indexOf(":30") >= 0) {
                            //var offset = new Date().getTimezoneOffset() / 60 * -1;
                            //var time = Number(d.substr(0, 2)) + offset + ":" + d.substr(3, 2) ;
                            return d;
                        }
                        // if ((i % 10) == 0) {}
                        else {
                            return null;
                        }
                    }
                })
            )
            .selectAll("text")
            .style("text-anchor", "end")
            .attr("dx", "-.8em")
            .attr("dy", ".15em")
            .attr("transform", "rotate(-90)");


        // // add the y Axis
        window.d3.select("#data .y_events")
            .remove();

        svg.append("g")
            .attr("class", "y_events")
            .attr("transform", "translate("+ (margin.left+3) + "," + 5 + ")")
            .call(
                window.d3.axisLeft(y)
            );

       var tooltip = window.d3.select(".tooltip");

       // enter and update selection
       bars
         .enter().append("rect")
         .merge(bars)
         .attr("x", function(d) { return x(d.label)-5; })
         .attr("width", x.bandwidth())
         .attr("y", function(d) { return y(d.weight)+5; })
         .attr("height", function(d) { 
         	if(d.weight === 0) { return 0}
         	return height - y(d.weight); 
         })
         .on("mousemove", function(d){
             // console.log("sdf");
             window.d3.select(this).attr("fill", "#00E062");
             var startTime = "";
             var endTime = "";

             if (typeof(d.label) === "string") {
                 startTime = Number(d.label.substr(0, 2))  + ":" + d.label.substr(3, 2);
                 endTime = Number(d.label.substr(0, 2)) + ":" + draw.leadingZero(Number(d.label.substr(3, 2)) + 5);
                 if (Number(d.label.substr(3, 2)) === 55 ) {
                     endTime = Number(d.label.substr(0, 2))+1 + ":00";
                 }
             }
             else {
                 startTime = d.label;
             }

            tooltip
              .style("left", window.d3.event.pageX - 50 + "px")
              .style("top", window.d3.event.pageY - 60 + "px")
              .style("display", "inline-block")
              .html((startTime) + "-" + endTime + "<br>" + "events: " + (d.weight) + "x");
          })
    	  .on("mouseout", function(){
              window.d3.select(this).attr("fill", function() {
                 return "";
              });
              tooltip.style("display", "none");
          });
         // .on("click", clicked);
         // .text(function (d) {return d.label;});

       // exit selection
       bars
           .exit().remove();

       if (!draw.pageLoaded ) {
           if (document.querySelectorAll(".databox").length >= 1) {
               document.querySelector(".databox").scrollLeft = 99000;
           }
       }

       draw.pageLoaded = true;

    }
};

draw.index();

window.draw = draw;

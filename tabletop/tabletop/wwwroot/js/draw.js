
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
        if (!isNaN(updateInterval) && updateInterval !== null && updateInterval !== undefined ) {
            setInterval(function(){
                draw.start();
            }, updateInterval);
        }

    },

    start : function () {
        // var url = "api/getRecentByName?name=" + window.envName;
        // var url = "EventsOfficehours_name_tafelvoetbal_date_0.json?" + Date.now();
        var url = window.drawEnv.url;
    	draw.loadJSON(url,
    			 function(data) {
                     data = draw.checkData(data);
                     draw.unHideUpdateElements(data);
                     // data = draw.compareData(data);
                     draw.drawD3(data);
                 },
                function(xhr) { console.error(xhr); }
       );
   },

    checkData : function(data){
        draw.data = data;
        console.log(draw.data);

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
    unHideUpdateElements : function(data){
        if (data === null) {
            alert("data fails");
            return;
        }
        document.querySelector('.databox').style.display = "block";
        document.querySelector('#preloader').style.display = "none";

        console.log(window.drawEnv.relativeDate);
        if (window.drawEnv.relativeDate == 0) {
            document.querySelector('#data').classList.add("redborder");
            console.log("hi");
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
       var utc_timestamp_unix = Date.UTC(now.getUTCFullYear(),now.getUTCMonth(), now.getUTCDate() ,
            now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds())/1000;
       return utc_timestamp_unix;
   },


    drawD3: function(data) {
        if (data === null) {
            console.log("no update");
            return;
        }
        console.log(data);

       var margin = {top: 40, right: 0, bottom: 30, left: 30},
       width = 800 + (margin.right - margin.left),
       height = 300 + (margin.top - margin.bottom);

       d3.select("#data")
           .attr("preserveAspectRatio", "xMinYMin meet")
           .attr("viewBox", "0 0 " + (width + margin.left + margin.right+5) + " " + (height +margin.top));

       var bars = d3.select("#data .bars")
           // .attr("height", height + margin.top + margin.bottom)
           .attr("fill", "#000")
           .attr("transform","translate(" + (margin.left+15) + "," + 0 + ")")
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

       svg = d3.select("#data .x_time")
           .remove();

       // add the x Axis
       svg = d3.select("#data");
       svg.append("g")
           .attr("class", "x_time")
           .attr("transform", "translate("+ (margin.left+5) + "," + height + ")")
           .attr("stroke", "#000")
           .call(
               d3.axisBottom(x)
                .tickFormat(function(d,i) {
                    if (d.indexOf(":00") >= 0 || d.indexOf(":30") >= 0) {
                        var offset = new Date().getTimezoneOffset() / 60 * -1;
                        var time = Number(d.substr(0, 2)) + offset + ":" + d.substr(3, 2) ;
                        return time;
                    }
                    // if ((i % 10) == 0) {}
                    else {
                        return null;
                    }
                })
            )
            .selectAll("text")
            .style("text-anchor", "end")
            .attr("dx", "-.8em")
            .attr("dy", ".15em")
            .attr("transform", "rotate(-90)");


        // // add the y Axis
        d3.select("#data .y_events")
            .remove();

        svg.append("g")
            .attr("class", "y_events")
            .attr("transform", "translate("+ (margin.left+3) + "," + 0 + ")")
            .attr("stroke", "#000")
            .call(d3.axisLeft(y));


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

       if (!draw.pageLoaded ) {
           if (document.querySelectorAll(".databox").length >= 1) {
               document.querySelector(".databox").scrollLeft = 99000;
           }
       }

       draw.pageLoaded = true;


    }
};

draw.index();

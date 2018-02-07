var isUserActive = true;

function idle() {
	var t;
	// Thanks: http://stackoverflow.com/questions/667555/detecting-idle-time-in-javascript-elegantly

	// reset the screensaver if you do something;
	window.onload = resetTimer;
	window.onmousemove = resetTimer;
	window.onmousedown = resetTimer; // catches touchscreen presses
	window.onclick = resetTimer;     // catches touchpad clicks
	window.onscroll = resetTimer;    // catches scrolling with arrow keys
	window.onkeypress = resetTimer;

    function idleHelper() {
        window.updateIsFreeEnv.isUserActive = false;
        console.log("Not active on screen");
	}

	function resetTimer() {
		clearTimeout(t);
        t = setTimeout(idleHelper, 90000);  // 90000 - time is in milliseconds 188400 == 3,14 minute

        if (!window.updateIsFreeEnv.isUserActive) {
			console.log("User is back again");

            window.signalr.pongSend();

            setTimeout(function() {
                var longTimeAgo = window.signalr.pongSend();
                if (longTimeAgo) {
                    // reset and update data
                    window.updateIsFree.updateManualData();
                    window.draw.start();
                    window.signalr.connection = null;
                    window.signalr.signalr();
                }
            }, 130);
		}

	    window.updateIsFreeEnv.isUserActive = true;
	}
}

idle();

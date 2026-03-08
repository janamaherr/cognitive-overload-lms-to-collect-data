document.addEventListener("mousemove", function(e){

    let data = {
        eventType: "mouseMove",
        x: e.clientX,
        y: e.clientY,
        timestamp: Date.now()
    };

    logBehavior(data);

});
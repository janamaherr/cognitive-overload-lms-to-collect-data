let lastKeyTime = Date.now();

document.addEventListener("keydown", function(e){

    let now = Date.now();

    let pause = now - lastKeyTime;

    lastKeyTime = now;

    logBehavior({
        eventType: "keyPress",
        key: e.key,
        pause: pause,
        timestamp: now
    });

});
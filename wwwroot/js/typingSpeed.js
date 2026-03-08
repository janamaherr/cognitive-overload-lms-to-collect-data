let lastKeyTime = null;

document.addEventListener("keydown", function(e){
    if (e.repeat) return; // ignore auto-repeat from holding a key
    if (!(e.key.length === 1 || e.key === ' ')) return; // ignore control keys

    const now = Date.now();

    if (!lastKeyTime) {
        lastKeyTime = now; // prime the timer, don't log the first interval
        return;
    }

    const pause = now - lastKeyTime;
    lastKeyTime = now;

    const cps = pause > 0 ? 1000 / pause : 0; // chars per second
    const cpm = cps * 60; // chars per minute

    logBehavior({
        eventType: "keyPress",
        key: e.key,
        pause: pause,
        cps: cps,
        cpm: cpm,
        timestamp: now
    });

});

// let lastKeyTime = Date.now();

// document.addEventListener("keydown", function(e){

//     let now = Date.now();

//     let pause = now - lastKeyTime;

//     lastKeyTime = now;

//     logBehavior({
//         eventType: "keyPress",
//         key: e.key,
//         pause: pause,
//         timestamp: now
//     });

// });
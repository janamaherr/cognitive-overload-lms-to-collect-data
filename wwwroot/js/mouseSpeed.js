(function(){

    let lastSample = null; // {x,y,timestamp}
    let latestEvent = null;
    let rafScheduled = false;

    function sendSample(sample, speed, dx, dy, dt){
        const data = {
            eventType: "mouseMove",
            x: sample.x,
            y: sample.y,
            timestamp: sample.timestamp,
            speedPxPerSec: speed,
            dx: dx,
            dy: dy,
            dtMs: dt
        };
        logBehavior(data);
    }

    document.addEventListener('mousemove', function(e){
        latestEvent = { x: e.clientX, y: e.clientY, timestamp: Date.now() };
        if(!rafScheduled){
            rafScheduled = true;
            requestAnimationFrame(processMove);
        }
    });

    function processMove(){
        rafScheduled = false;
        if(!latestEvent) return;

        if(!lastSample){
            lastSample = latestEvent;
            sendSample(latestEvent, 0, 0, 0, 0);
            return;
        }

        const dx = latestEvent.x - lastSample.x;
        const dy = latestEvent.y - lastSample.y;
        const dt = latestEvent.timestamp - lastSample.timestamp; // ms
        const dist = Math.hypot(dx, dy); // pixels
        const speed = dt > 0 ? (dist / dt) * 1000 : 0; // px / s

        sendSample(latestEvent, speed, dx, dy, dt);
        lastSample = latestEvent;
    }

})();
function logBehavior(data){
    const url = "/Behavior/LogBehavior";
    const body = JSON.stringify(data);

    if (navigator.sendBeacon) {
        try {
            const blob = new Blob([body], { type: "application/json" });
            const ok = navigator.sendBeacon(url, blob);
            if (ok) return;
        } catch (e) {
            // fall back to fetch below
        }
    }

    fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: body,
        keepalive: true
    }).catch(err => {
        console.error('logBehavior failed:', err);
    });

}
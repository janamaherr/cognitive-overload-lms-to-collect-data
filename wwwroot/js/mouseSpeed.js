class MouseSpeedTracker {
    constructor() {
        this.positions = [];
        this.isTracking = false;
        this.samples = [];
    }
    
    startTracking() {
        this.isTracking = true;
        this.positions = [];
        this.samples = [];
        
        this.trackInterval = setInterval(() => {
            if (this.isTracking && this.positions.length > 1) {
                this.calculateSpeed();
            }
        }, 100);
    }
    
    stopTracking() {
        this.isTracking = false;
        if (this.trackInterval) {
            clearInterval(this.trackInterval);
        }
    }
    
    calculateSpeed() {
        if (this.positions.length < 2) return;
        
        const latest = this.positions[this.positions.length - 1];
        const previous = this.positions[this.positions.length - 2];
        
        const deltaTime = (latest.timestamp - previous.timestamp) / 1000; // in seconds
        if (deltaTime <= 0) return;
        
        const deltaX = latest.x - previous.x;
        const deltaY = latest.y - previous.y;
        const distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
        const speed = distance / deltaTime; // pixels per second
        
        this.samples.push({
            timestamp: latest.timestamp,
            speed: speed,
            x: latest.x,
            y: latest.y
        });
    }
    
    getData() {
        return {
            movements: this.samples,
            averageSpeed: this.samples.length > 0 
                ? this.samples.reduce((sum, s) => sum + s.speed, 0) / this.samples.length 
                : 0,
            maxSpeed: this.samples.length > 0 
                ? Math.max(...this.samples.map(s => s.speed)) 
                : 0,
            totalSamples: this.samples.length
        };
    }
}

// Export for use in other files
window.MouseSpeedTracker = MouseSpeedTracker;
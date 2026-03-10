class BehaviorLogger {
    constructor(sessionId, sectionNumber) {
        this.sessionId = sessionId;
        this.sectionNumber = sectionNumber;
        this.gameStartTime = null;
        this.gameEndTime = null;
        this.mouseMovements = [];
        this.typingEvents = [];
        this.headPositions = [];
        this.hesitationPauses = [];
        this.lastMousePosition = { x: 0, y: 0, timestamp: Date.now() };
        this.lastKeyTimestamp = Date.now();
        this.pauseThreshold = 2000; // 2 seconds considered a pause
        
        this.setupEventListeners();
    }
    
    startGame(gameType) {
        this.gameStartTime = new Date().toISOString();
        this.gameType = gameType;
        console.log(`Game started: ${gameType}`);
    }
    
    endGame() {
        this.gameEndTime = new Date().toISOString();
        
        // Detect pauses at the end
        this.detectHesitationPauses();
        
        return {
            mouseMovements: this.mouseMovements,
            typingEvents: this.typingEvents,
            headPositions: this.headPositions,
            hesitationPauses: this.hesitationPauses,
            averageMouseSpeed: this.calculateAverageMouseSpeed(),
            averageTypingSpeed: this.calculateAverageTypingSpeed(),
            averageHeadMovement: this.calculateAverageHeadMovement()
        };
    }
    
    setupEventListeners() {
        // Mouse movement tracking
        document.addEventListener('mousemove', (e) => {
            const currentTime = Date.now();
            const deltaTime = (currentTime - this.lastMousePosition.timestamp) / 1000; // in seconds
            
            if (deltaTime > 0) {
                const deltaX = e.clientX - this.lastMousePosition.x;
                const deltaY = e.clientY - this.lastMousePosition.y;
                const distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
                const speed = distance / deltaTime; // pixels per second
                
                this.mouseMovements.push({
                    timestamp: new Date(currentTime).toISOString(),
                    x: e.clientX,
                    y: e.clientY,
                    speed: speed,
                    acceleration: this.calculateAcceleration(speed, currentTime)
                });
            }
            
            this.lastMousePosition = {
                x: e.clientX,
                y: e.clientY,
                timestamp: currentTime
            };
        });
        
        // Typing tracking
        document.addEventListener('keydown', (e) => {
            const currentTime = Date.now();
            const delaySinceLastKey = currentTime - this.lastKeyTimestamp;
            
            // Calculate typing speed (keys per minute)
            const speed = delaySinceLastKey > 0 ? 60000 / delaySinceLastKey : 0;
            
            this.typingEvents.push({
                timestamp: new Date(currentTime).toISOString(),
                key: e.key,
                delaySinceLastKeyMs: delaySinceLastKey,
                speed: speed
            });
            
            this.lastKeyTimestamp = currentTime;
        });
        
        // Check for hesitation pauses
        setInterval(() => {
            this.detectHesitationPauses();
        }, 1000);
    }
    
    detectHesitationPauses() {
        const currentTime = Date.now();
        const timeSinceLastMouse = currentTime - this.lastMousePosition.timestamp;
        const timeSinceLastKey = currentTime - this.lastKeyTimestamp;
        
        if (timeSinceLastMouse > this.pauseThreshold || timeSinceLastKey > this.pauseThreshold) {
            // Check if we're already in a pause
            const lastPause = this.hesitationPauses[this.hesitationPauses.length - 1];
            
            if (!lastPause || lastPause.endTime) {
                // Start new pause
                this.hesitationPauses.push({
                    startTime: new Date(currentTime - Math.min(timeSinceLastMouse, timeSinceLastKey)).toISOString(),
                    endTime: null,
                    durationSeconds: null,
                    location: window.location.pathname
                });
            }
        } else {
            // End any ongoing pause
            const lastPause = this.hesitationPauses[this.hesitationPauses.length - 1];
            if (lastPause && !lastPause.endTime) {
                lastPause.endTime = new Date().toISOString();
                lastPause.durationSeconds = (new Date(lastPause.endTime) - new Date(lastPause.startTime)) / 1000;
            }
        }
    }
    
    logMouseClick(target) {
        this.mouseMovements.push({
            timestamp: new Date().toISOString(),
            x: this.lastMousePosition.x,
            y: this.lastMousePosition.y,
            speed: 0,
            acceleration: 0,
            isClick: true,
            target: target
        });
    }
    
    calculateAverageMouseSpeed() {
        if (this.mouseMovements.length === 0) return 0;
        const totalSpeed = this.mouseMovements.reduce((sum, m) => sum + m.speed, 0);
        return totalSpeed / this.mouseMovements.length;
    }
    
    calculateAverageTypingSpeed() {
        if (this.typingEvents.length === 0) return 0;
        const totalSpeed = this.typingEvents.reduce((sum, t) => sum + t.speed, 0);
        return totalSpeed / this.typingEvents.length;
    }
    
    calculateAverageHeadMovement() {
        // Placeholder for MediaPipe integration
        return 0;
    }
    
    calculateAcceleration(currentSpeed, currentTime) {
        if (this.mouseMovements.length < 2) return 0;
        
        const lastMovement = this.mouseMovements[this.mouseMovements.length - 1];
        if (!lastMovement) return 0;
        
        const timeDiff = (currentTime - new Date(lastMovement.timestamp).getTime()) / 1000;
        const speedDiff = currentSpeed - lastMovement.speed;
        
        return timeDiff > 0 ? speedDiff / timeDiff : 0;
    }
}

// Export for use in other files
window.BehaviorLogger = BehaviorLogger;
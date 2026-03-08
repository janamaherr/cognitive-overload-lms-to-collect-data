function logBehavior(data){

    fetch("/Behavior/LogBehavior", {
        method: "POST",
        headers:{
            "Content-Type":"application/json"
        },
        body: JSON.stringify(data)
    });

}
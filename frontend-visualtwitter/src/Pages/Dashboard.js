import { FormControl, Box, TextField, Button } from '@mui/material';

import React, { useState } from 'react';


const Dashboard = () => {

    const handleSubmit = (e) => {
        e.preventDefault();
    }
    const [event, setEvent] = useState({
        'event': ""
    })

    return (
        <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center",minHeight:"35vh" }}>
            <FormControl>
                <Box sx={{ display: "flex", direction: "inline"}}>
                    <TextField label="Search BasketBall event " value={event.event} required onChange={(e) => { setEvent({ ...event, event: e.target.value }) }} />
                    <Button onClick={handleSubmit} variant="contained" color="primary">Search</Button>
                </Box>
            </FormControl>
        </Box>


    )
}
export { Dashboard };
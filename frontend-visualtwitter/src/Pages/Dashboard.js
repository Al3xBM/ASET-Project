import { FormControl, Box, TextField, Button } from '@mui/material';

import React, { useState } from 'react';
import { SearchResult } from '../Components/SearchResult';


const Dashboard = () => {

    const handleSubmit = (e) => {
        e.preventDefault();
        setShowSearchResult(true);
    }
    const [event, setEvent] = useState({
        'event': ""
    })
    const [showSearchResult,setShowSearchResult] = useState(false);

    return (

        <Box sx={{ display: "flex",flexDirection:"column", alignItems: "center", justifyContent: "center", minHeight: "35vh" }}>
            <FormControl>
                <Box sx={{ display: "flex", direction: "inline" }}>
                    <TextField label="Search BasketBall event " value={event.event} required onChange={(e) => { setEvent({ ...event, event: e.target.value }) }} />
                    <Button onClick={handleSubmit} variant="contained" color="primary">Search</Button>
                </Box>
            </FormControl>
            <Box sx = {{display:"flex",  justifyContent: "center",flexDirection:"inline",width:"600px"}}>
            
            
            </Box>
            
        </Box>





    )
}
export { Dashboard };
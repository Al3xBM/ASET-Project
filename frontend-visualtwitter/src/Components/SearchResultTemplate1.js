import { FormLabel,Box } from '@mui/material';

import React, { useContext, useEffect } from 'react';
import { SearchResultTemplate1Context } from '../Contexts/SearchResultTemplate1Context';


const SearchResultTemplate1=()=>
{
    const { searchResult } = useContext(SearchResultTemplate1Context);
    console.log("searchResult: ",searchResult);
    return(
    <>
        <Box sx={{display:"flex", flexDirection:"column", border:"0.5px solid grey",width:"400px"}}>
            <FormLabel>Team1: {searchResult.team1 ?? searchResult.team1}</FormLabel>
            <FormLabel>Team2: {searchResult.team2 ?? searchResult.team2}</FormLabel>
            <FormLabel>Team1Score: {searchResult.team1Score ?? searchResult.team1Score}</FormLabel>
            <FormLabel>Team2Score: {searchResult.team2Score ?? searchResult.team2Score}</FormLabel>
            <FormLabel>Team1MVP: {searchResult.team1MVP.name ?? searchResult.team1MVP.name}</FormLabel>
            <FormLabel>Team2MVP: {searchResult.team2MVP.name ?? searchResult.team2MVP.name}</FormLabel>
            <FormLabel>OtherInfo: {searchResult.otherInfo ?? searchResult.otherInfo}</FormLabel>
        </Box>
        <Box sx={{display:"flex", flexDirection:"column", border:"0.5px solid grey",width:"400px"}}>
            <FormLabel>RelatedTweets:</FormLabel>
        </Box>

   </>
    )
}

export { SearchResultTemplate1 };
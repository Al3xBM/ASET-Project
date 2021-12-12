import { createContext, useState } from "react";
export const SearchResultTemplate1Context=createContext();
export const SearchResultTemplate1ContextProvider=(props)=>{
    var [searchResult,setSearchResult]=useState(null);
   
    return(
        <SearchResultTemplate1Context.Provider value={{ searchResult, setSearchResult }}>
            {props.children}
        </SearchResultTemplate1Context.Provider>
    )
}
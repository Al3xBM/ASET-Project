import { Navbar } from './Components/Navbar';
import { SearchResultTemplate1ContextProvider } from './Contexts/SearchResultTemplate1Context';


function App() {
  return (
    <SearchResultTemplate1ContextProvider>
      <Navbar/>
    </SearchResultTemplate1ContextProvider>
     
  );
}

export default App;
import { useEffect, useState, createContext } from 'react';
import './App.css';
import { DemoContextProvider } from './Components/DemoContextProvider';
import { LevelThree } from './Components/LevelThree';
import { AdvancedCalc } from './Components/AdvancedCalc';
import { BasicPureComponentDemo } from './Components/BasicPlaceholderPureComponent';
import { FactsButton } from './Components/FactComponent';


function App() {
    return (
        <div>
            <FactsButton />
            {/*<BasicPureComponentDemo />*/}
            {/*<AdvancedCalc />*/}
            {/*<DemoContextProvider />*/}
            {/*<LevelThree />*/}
        </div>
    );
}

export default App;
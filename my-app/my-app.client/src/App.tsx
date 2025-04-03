import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import $ from 'jquery';
import 'bootstrap/dist/js/bootstrap.bundle.min';
import { DemoContextProvider } from './Components/DemoContextProvider';
import { LevelThree } from './Components/LevelThree';
import { AdvancedCalc } from './Components/AdvancedCalc';
import { BasicPureComponentDemo } from './Components/BasicPlaceholderPureComponent';
import { FactsButton } from './Components/FactComponent';

import { NavigateApp } from './Routes/NavigateApp';
import CurrencyRates from './Components/CurrencyRatesAdvanced';

const App: React.FC = () => {
    return (
        <div>
            {/* <BasicApp /> */}
            {/* <RouteParamsApp /> */}
            {/* <UseNavigateApp /> */}
            {/* <OutletApp /> */}
            <NavigateApp />
            <CurrencyRates />
            {/*<FactsButton />*/}
            {/*<BasicPureComponentDemo />*/}
            {/*<AdvancedCalc />*/}
            {/*<DemoContextProvider />*/}
            {/*<LevelThree />*/}
        </div>
    );
};

export default App;
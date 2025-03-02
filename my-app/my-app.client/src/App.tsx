import { useEffect, useState, createContext } from 'react';
import './App.css';

interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

interface Fact {
    current_page: number;// 1,
    data: string;
    first_page_url: string;// "https://catfact.ninja/facts?page=1",
    from: number;// 1,
    last_page: number;// 34,
    last_page_url: number;//"https://catfact.ninja/facts?page=34",
    links: string;
    next_page_url: string;// "https://catfact.ninja/facts?page=2",
    path: string;//"https://catfact.ninja/facts",
    per_page: number;//10,
    prev_page_url: string;//null,
    to: number;//10,
    total: number;//332
}

const DemoContext = createContext({ message: '????', updateMessage: (msg: string) => { } });

function MyComponent({ fact }) {
    //const [facts, setFacts] = useState<Fact[]>();
    //const level = useContext(LevelContext);
    return (
        <div>
            fact
        </div>
    );
}

function MyButton() { 
    const [facts, setFacts] = useState<Fact[]>();

    async function populateNinjaFactsData() {
        const response = await fetch('https://catfact.ninja/facts');
        if (response.ok) {
            const data = await response.json();
            setFacts(data);

        }

        const DemoContext = createContext(facts);
        console.log(facts);
    }

    //updateMessage = (msg: string) => {
    //    console.log('DemoContextProvider.updateMessage:', msg);
    //    this.setState({ message: facts });
    //};

    function handleClick() {
        alert('You clicked me!');
    }

    return (
        <div>
            <button onClick={populateNinjaFactsData}>
                Click me
            </button>
            <MyComponent fact={facts[0]}/>
        </div>
    );
}


function App() {
    const [forecasts, setForecasts] = useState<Forecast[]>();

    useEffect(() => {
        populateWeatherData();
    }, []);

    const contents = forecasts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tableLabel">Weather forecast</h1>

            <MyComponent />
            <MyButton />

            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );

    async function populateWeatherData() {
        const response = await fetch('weatherforecast');
        if (response.ok) {
            const data = await response.json();
            setForecasts(data);
        }
    }

}

export default App;
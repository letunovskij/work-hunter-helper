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
    first_page_url?: string;// "https://catfact.ninja/facts?page=1",
    from?: number;// 1,
    last_page?: number;// 34,
    last_page_url?: number;//"https://catfact.ninja/facts?page=34",
    links?: string;
    next_page_url?: string;// "https://catfact.ninja/facts?page=2",
    path?: string;//"https://catfact.ninja/facts",
    per_page?: number;//10,
    prev_page_url?: string;//null,
    to?: number;//10,
    total?: number;//332
}

var DemoContext = createContext({ message: 'Тест', updateMessage: (msg: string) => { } });

function MyComponent({ facts }: { facts: Fact[] | undefined }) {
    //const [stateFacts, setStateFacts] = useState<Fact[] | undefined>(facts);
    //setStateFacts(facts);

    return (
        <div>
            facts[0].current_page
        </div>
    );
}

//const dataHandler = (data: dataType) => { работаем с данными }

function MyButton() { 
    const [facts, setFacts] = useState<Fact[]>();

    async function populateNinjaFactsData(): Promise<Fact[] | undefined> {
        const response = await fetch('https://catfact.ninja/facts');
        if (response.ok) {
            const data = await response.json();
            setFacts(data);
        }
        setFacts([{ data: "test", current_page: 200 }] );
        const DemoContext = createContext(facts);
        console.log(facts);

        return facts;
    }

    return (
        <div
            style={{
                color: 'green'
            }}>
            <button onClick = { () => populateNinjaFactsData() }>
                Click me
            </button>
            <MyComponent facts={facts}/>
        </div>
    );
}


function App() {
    return (
        <div>
            <h1 id="tableLabel">Test React (useState is not compiled to js)</h1>
            <MyButton />
        </div>
    );
}

export default App;
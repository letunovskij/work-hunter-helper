import { Component, createContext } from 'react';

interface DemoContextStateContract {
    message: string;
}

// Создаем контекст
const DemoContext = createContext({ message: 'Сообщение из Context!', updateMessage: (msg: string) => { } });


class DemoContextComponent extends Component<{}, DemoContextStateContract> {
    constructor(props: {}) {
        super(props);

        this.state = {
            message: 'Сообщение из DemoContextProvider!'
        };

    }

    updateMessage = (msg: string) => {
        console.log('DemoContextProvider.updateMessage:', msg);
        this.setState({ message: msg });
    };

    render() {
        return (
            <DemoContext.Provider
                value={{
                    message: this.state.message,
                    updateMessage: this.updateMessage
                }}
            >
                <h1>Пример использования Context</h1>
            </DemoContext.Provider>
        );
    }
}

export { DemoContextComponent as DemoContextProvider, DemoContext };
//function MyButton() {
//    const [facts, setFacts] = useState<Fact[]>();

//    async function populateNinjaFactsData(): Promise<Fact[] | undefined> {
//        const response = await fetch('https://catfact.ninja/facts');
//        if (response.ok) {
//            const data = await response.json();
//            setFacts(data);
//        }
//        setFacts([{ data: "test", current_page: 200 }]);
//        const DemoContext = createContext(facts);
//        console.log(facts);

//        return facts;
//    }

//    return (
//        <div
//            style={{
//                color: 'green'
//            }}>
//            <button onClick={() => populateNinjaFactsData()}>
//                Click me
//            </button>
//            <MyComponent facts={facts} />
//        </div>
//    );
//}
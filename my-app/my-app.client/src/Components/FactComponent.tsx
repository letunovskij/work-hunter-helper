import React, { PureComponent } from 'react';

interface FactData {
    fact: string;
    length: number
}

interface Fact {
    current_page: number;// 1,
    data: FactData[];
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

interface Props {
    fact: Fact | undefined;
}

//https://ru.legacy.reactjs.org/docs/react-api.html#reactpurecomponent

class FactsComponent extends PureComponent<Props> {
    render() {
        console.log('Render: FactsComponent рендерится');
        let displayFactsCount: number | undefined = -1;
        if (this.props.fact != undefined) {
            displayFactsCount = this.props.fact?.total;
            return (
                <div>

                    <h1
                        style={{
                            color: 'green'
                        }}>Количество обнаруженных фактов, {displayFactsCount}!</h1>

                    <table>
                        <thead>
                            <tr>
                                <th>Fact Length</th>
                                <th>Fact</th>
                            </tr>
                        </thead>
                        <tbody>
                            {this.props.fact.data.map(fact =>
                                <tr key={fact.fact}>
                                    <td>{fact.length}</td>
                                    <td>{fact.fact}</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>
            );
        }
        else
        {
            return (
                <div>
                    <h1
                        style={{
                            color: 'red'
                        }}>Фактов не обнаружено, {displayFactsCount}!</h1>
                </div>
            )
        };
    }
}

export class FactsButton extends React.Component {
    state = {
        facts: undefined
    };

    populateFacts = async (): Promise<Fact[] | undefined> => { 
        const response = await fetch('https://catfact.ninja/facts');
        if (response.ok) {
            const data = await response.json();
            this.setState({ facts: data });
        } else {
            console.log(response.text);
        }
        console.log(this.state.facts);

        return this.state.facts;
    }

    render() {
        return (
            <div>
                <button onClick={() => this.populateFacts()}>
                    Click for Populates Facts
                </button>
                <FactsComponent fact={this.state.facts} />
            </div>
        );
    }
}
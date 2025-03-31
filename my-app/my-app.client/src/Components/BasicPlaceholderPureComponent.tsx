import React, { PureComponent } from 'react';

interface Props {
    name: string;
}

//https://ru.legacy.reactjs.org/docs/react-api.html#reactpurecomponent
class BasicPlaceholderPureComponent extends PureComponent<Props> {
    render() {
        console.log('Render: BasicPlaceholderPureComponent рендерится');
        return (
            <div>
                <h1>Привет, {this.props.name}!</h1>
            </div>
        );
    }
}

export class BasicPureComponentDemo extends React.Component {
    state = {
        name: 'Мир'
    };

    changeName = () => {
        this.setState({ name: this.state.name == 'React' ? 'Мир' : 'React' });
    };

    render() {
        return (
            <div>
                <button onClick={this.changeName}>Изменить имя</button>
                <BasicPlaceholderPureComponent name={this.state.name} />
            </div>
        );
    }
}
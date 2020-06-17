import * as React from 'react';
import '../index.css';
import httpClient from '../httpClient';
import Spinner from './Spinner';
const config = require('../config.json');


class Home extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isLoading: false,
            data: null
        }
        
        this.ensureDataFetched = this.ensureDataFetched.bind(this);
        this.setLoading = this.setLoading.bind(this);
    }


     setLoading (isLoading){
        this.setState({ isLoading });
    }



    componentDidMount() {
    }

    ensureDataFetched() {

    this.setLoading(true);


        const client = new httpClient();
        return client.get(`${config.aws.api.url}${config.aws.api.path}`, { 'x-api-key': 'nHtKKfZoy98mdnCM0fBYsa55UzyrXCPS4vSmELjb' })
            .then(response => {
                const data = JSON.parse(response.data.body);

                if (data && response.data && response.status === 200) {

                    this.setState((state) => {
                        return { state: state.data = data };
                    });
                }
                this.setLoading(false);
            }).catch((err) => {this.setLoading(false);});
    }

    render() {
        return (
            <React.Fragment>
                <div>
                    <h1>Hello, world!</h1>
                    <input type="button" onClick={this.ensureDataFetched} value="get data"></input>
                </div>
                <Spinner isLoading={this.state.isLoading} />
            </React.Fragment>
        )
    };
};

export default Home;
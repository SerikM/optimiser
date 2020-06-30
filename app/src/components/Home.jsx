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
            breaks: null,
            breaksWithCommercials: null,
            total: 0,
            errors: false
        }

        this.ensureDataFetched = this.ensureDataFetched.bind(this);
        this.refetchData = this.refetchData.bind(this);
        this.setLoading = this.setLoading.bind(this);
    }


    setLoading(isLoading) {
        this.setState({ isLoading });
    }

    componentDidMount() {
        this.ensureDataFetched()
            .then(() => {
                this.refetchData();
            });
    }

    ensureDataFetched() {
        this.setLoading(true);
        const client = new httpClient();
        return client.get(`${config.aws.api.url}${config.aws.api.path}`, { 'x-api-key': config.aws.api.key })
            .then(response => {
                if (response.data && response.status === 200) {
                    this.setState((state) => {
                        return { state: state.breaks = response.data };
                    });
                }
                this.setLoading(false);
            })
            .catch((err) => { this.setLoading(false); });
    }

    refetchData() {
        this.setLoading(true);
        const client = new httpClient();
        return client.post(`${config.aws.api.url}${config.aws.api.path}`, this.state.breaks, { 'x-api-key': config.aws.api.key })
            .then(response => {
                if (response.data && response.status === 200) {
                    this.setState((state) => {
                        return { state: state.breaksWithCommercials = response.data.breaksWithCommercials, total: response.data.total };
                    });
                }
                this.setLoading(false);
            })
            .catch((err) => { this.setLoading(false); });
    }

    updateRating(target, brId, demoName, elId) {
        this.setState({ current: elId });
        let value = target.value;
        const number = parseInt(value, 10);
        if (value.length > 6 || isNaN(number) || ! /^\d+$/.test(value)) {
            target.style.borderColor = "red";
            this.setState({ errors: true });
            return;
        }
        else {
            this.setState({ errors: false });
            target.style.borderColor = "";
        }

        const breaks = this.state.breaks;
        breaks.map(p => p.Id === brId ? p.Ratings.map(d => d.DemoName === demoName ? d.Score = number : d) : p);
        this.setState((state) => {
            return { state: state.breaks = breaks };
        });
        this.refetchData();
    }


    render() {
        return (
            <React.Fragment>

                <div className="total">
                    {this.state.isLoading ? <Spinner isLoading={this.state.isLoading} /> :
                        this.state.total > 0 && <h2>{this.state.total}</h2>
                    }
                </div>
                <hr></hr>
                <div className="main">
                    <div className="row row-header">
                        <div className="col-2">
                            <div className="row">
                                <div className="col-12">Break</div>
                            </div>
                        </div>
                        <div className="col-10">
                            <div className="row">
                                <div className="col-7 col-md-8">Demographic</div>
                                <div className="col-5 col-md-4">Rating</div>
                            </div>
                        </div>
                    </div>
                    {this.state.breaks && this.state.breaks.map((br, i) =>
                        <div key={i} className="row align-items-center">
                            <div className="col-2">
                                <div className="row">
                                    <div key={i + 1} className="col-12" >{br.Id}</div>
                                </div>
                            </div>
                            <div className="col-10">
                                <div className="row">
                                    {br.Ratings && br.Ratings.map((rating, j) =>
                                        <React.Fragment key={j + 5}>
                                            <div key={j + 1} className="col-7 col-md-8">
                                                <span key={j + 2}></span>  {rating.DemoName}
                                            </div>

                                            <div key={j + 3} className="col-5 col-md-4">
                                                <input key={`${i}${j}`} type="number" disabled={this.state.errors && this.state.current !== `${i}${j}`} inputMode="numeric" onChange={(e) => this.updateRating(e.target, br.Id, rating.DemoName, `${i}${j}`)} defaultValue={rating.Score} />
                                            </div>
                                        </React.Fragment>
                                    )}
                                </div>
                            </div>
                        </div>
                    )}
                </div>
                <div className="main lower">
                    <React.Fragment>
                        <div className="row row-header">

                            <div className="col-2">
                                <div className="row">
                                    <div className="col-12">Break</div>
                                </div>
                            </div>

                            <div className="col-10">
                                <div className="row">
                                    <div className="col-4">Commercial</div>
                                    <div className="col-4">Demographic</div>
                                    <div className="col-4">Type</div>
                                </div>
                            </div>

                        </div>
                        {this.state.breaksWithCommercials && this.state.breaksWithCommercials.map((br, i) =>
                            <div key={i} className="row align-items-center">
                                <div className="col-2">
                                    <div className="row">
                                        <div key={i + 'a'} className="col-12" >{br.Id}</div>
                                    </div>
                                </div>
                                <div className="col-10">
                                    <div className="row">
                                        {br.Commercials && br.Commercials.map((comm, k) =>
                                            <React.Fragment key={k + 'e'}>
                                                <div key={k + 'b'} className="col-4">
                                                    {this.state.isLoading ? <Spinner isLoading={this.state.isLoading} /> :
                                                        comm.Id
                                                    }
                                                </div>
                                                <div key={k + 'c'} className="col-4">
                                                    {this.state.isLoading ? <Spinner isLoading={this.state.isLoading} /> :
                                                        comm.TargetDemoName
                                                    }
                                                </div>
                                                <div key={k + 'd'} className="col-4">
                                                    {this.state.isLoading ? <Spinner isLoading={this.state.isLoading} /> :
                                                        comm.CommercialTypeName
                                                    }
                                                </div>
                                            </React.Fragment>
                                        )}
                                    </div>
                                </div>
                            </div>
                        )}
                    </React.Fragment>
                </div>
            </React.Fragment>
        )
    };
};
export default Home;
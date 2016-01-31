import { connect } from 'react-redux'
import onSubmit from './actions'
import Helmet from 'react-helmet'
import Form from './Form'

class View extends React.Component {
  render() {
    const { props: { params, location: { query, }, }, } = this
    return(
      <div>
        <Helmet title="Verify" />
        <h2>Verify.</h2>
        <h4>Confirm your contact information.</h4>
        <hr />
        <Form formKey="verify" onSubmit={onSubmit} initialValues={{ ...params, ...query }} />
      </div>
    )
  }
}

const select = () => ({})
export default connect(select)(View)

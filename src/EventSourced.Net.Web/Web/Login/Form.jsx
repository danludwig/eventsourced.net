import { reduxForm } from 'redux-form'
import validate from './validation'
import ValidationSummary from '../Shared/ValidationSummary'

class Form extends React.Component {
  static propTypes = {
    fields: React.PropTypes.object.isRequired,
    handleSubmit: React.PropTypes.func.isRequired,
    submitting: React.PropTypes.bool.isRequired,
  };

  render() {
    const { fields: { login, password, }, handleSubmit, submitting, submitFailed } = this.props
    return(
      <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>
        <div className="form-group">
          <div className="col-md-4">
            <label className="control-label sr-only">Email address, phone number, or username</label>
            <input type="text" className="form-control" placeholder="Email address, phone number, or username" disabled={submitting} {...login} />
          </div>
        </div>
        <div className="form-group">
          <div className="col-md-4">
            <label className="control-label sr-only">Password</label>
            <input type="password" className="form-control" placeholder="Password" disabled={submitting} {...password} />
          </div>
        </div>
        <div className="form-group">
          <div className="col-md-10">
            <button type="submit" className="btn btn-default" disabled={submitting}>Log in</button>
          </div>
        </div>
        { submitFailed &&
          <ValidationSummary form={this.props} /> }
      </form>
    )
  }
}

const form = 'login'
const fields = ['login', 'password', 'returnUrl']
export default reduxForm({
  form,
  fields,
  validate,
})(Form)
